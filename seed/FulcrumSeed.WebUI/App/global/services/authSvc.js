angular.module('fulcrumSeed.global.services.auth', ['fulcrumSeed.global', 'LocalStorageModule'])
	.service('authTokenSvc', [
		'localStorageService',
		function (localStorageService) {

			var setToken = function (token) {
				//console.log('setting token', token);

				localStorageService.set("oauth_token", token);
			};
			var getToken = function() {
				var token = localStorageService.get("oauth_token");

				//console.log('reading token', token);

				return token;
			};
			var clearToken = function() {
				//console.log('clearing token');

				return localStorageService.remove("oauth_token");
			};

			this.getToken = function() {
				return getToken();
			};

			this.setToken = function (token) {
				setToken(token);
			};

			this.clearToken = function() {
				clearToken();
			};
		}
	])
	.service('authRedirectSvc', [
		'localStorageService',
		function(localStorageService) {
			var setRedirect = function(redirect) {
				localStorageService.set("auth_redirect", redirect);
			};
			var getRedirect = function() {
				return localStorageService.get("auth_redirect");
			};
			var clearRedirect = function() {
				//console.log('clearing auth_redirect');
				return localStorageService.remove("auth_redirect");
			};

			this.getRedirect = function() {
				return getRedirect();
			};

			this.setRedirect = function(state, params) {
				if (state.name != "login") {
					setRedirect({ state: state, params: params });
				}
			};

			this.clearRedirect = function() {
				clearRedirect();
			};
		}
	])
	.service('authSvc', [
		'$http', 'appSettings', 'authTokenSvc', '$q', '$rootScope',
			'$injector',
		function ($http, appSettings, authTokenSvc, $q, $rootScope,
			$injector) {

			var authorized = false;

			this.login = function(username, password) {
				var authRequest = {
					username: username,
					password: password,
					grant_type: 'password',
					client_id: 'FulcrumApi',
					client_secret: 'apiSecret',
					scope: 'openid FulcrumApiScope',
				};
				var deferred = $q.defer();

				$http.post(appSettings.apiBasePath + "auth/connect/token", $.param(authRequest), {
						headers: {
							'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
							'Authorization': 'Basic ' + btoa("FulcrumApi:apiSecret")
						}
					})
					.success(function(token) {
						authorized = true;
						authTokenSvc.setToken(token);

						$rootScope.$broadcast('authenticated');

						deferred.resolve(token);

						var redirectSvc = $injector.get("authRedirectSvc");
						var redirectTarget = redirectSvc.getRedirect();

						if (redirectTarget) {
							$injector.get("$state").go(redirectTarget.state.name, redirectTarget.params);
						} else {
							$injector.get("$state").go("home");
						}
					})
					.error(function (error) {
						console.log('authentication error', error);
						authorized = false;
						authTokenSvc.clearToken();
						// TODO: clear profile
						deferred.reject(error);
					});

				return deferred.promise;
			};

			this.attemptRefresh = function() {

				var deferred = $q.defer();

				var token = authTokenSvc.getToken();

				console.log('attempting to refresh token', token);

				if (!token) {
					deferred.reject("No refresh token found");
					return deferred.promise;
				}

				var authRequest = {
					refresh_token: token.refresh_token,
					grant_type: 'refresh_token',
					client_id: 'FulcrumApi',
					client_secret: 'apiSecret',
					scope: 'FulcrumApiScope',
				};

				// TODO: indicate this in the UI somehow
				$rootScope.$broadcast('authenticating');

				$http.post(appSettings.apiBasePath + "identity/connect/token", $.param(authRequest), {
						headers: {
							'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
						}
					})
					.success(function(token) {
						authorized = true;
						authTokenSvc.setToken(token);

						// cancel UI indicator in response to this
						$rootScope.$broadcast('authenticated');

						deferred.resolve(token);
					})
					.error(function(error) {
						console.log('refresh error', error);
						authorized = false;
						authTokenSvc.clearToken();
						$rootScope.$broadcast('authentication_failed', { error: error });
						deferred.reject(error);
					});

				return deferred.promise;
			};

			this.logout = function() {
				console.log('logging out');

				authTokenSvc.clearToken();
				// TODO: clear profile
			};

			this.isAuthorized = function() {
				if (authorized) {
					return true;
				} else {
					var token = authTokenSvc.getToken();

					if (token) {
						return true;
					}

					return false;
				}
			};
		}
	])
	.factory('authInterceptorSvc', [
		'$q', 'authTokenSvc', '$injector', '$rootScope',
		function($q, authTokenSvc, $injector, $rootScope) {

			var factory = {};

			factory.request = function(config) {

				config.headers = config.headers || {};

				var token = authTokenSvc.getToken();

				if (token) {
					config.headers.Authorization = 'Bearer ' + token.access_token;
				}

				return config;
			};

			factory.responseError = function(rejection) {
				// TODO: implement refresh tokens in idsvr3
				if (rejection.status === 401111) {
					var deferred = $q.defer();

					$injector.get("authSvc").attemptRefresh()
						.then(function() {
								// TODO: clean up request-resuming logic
								$injector.get("$http")(rejection.config).then(function(response) {
									// we have a successful response - resolve it using deferred
									$rootScope.$broadcast('authenticated');

									deferred.resolve(response);
									var redirectSvc = $injector.get("authRedirectSvc");
									var redirectTarget = redirectSvc.getRedirect();
									//console.log('redirectTarget', redirectTarget);
									//console.log('redirectTarget.state', redirectTarget.state);
									$injector.get("$state").go(redirectTarget.state.name, redirectTarget.params);
								}, function(response) {
									deferred.reject(response); // retried request failed
									$injector.get("$state").go("login");
								});
							}, function(response) {
								// unable to refresh auth token, prompt for credentials
								$injector.get("$state").go("login");
								return;
							}
						);
					return deferred.promise;
				}

				return $q.reject(rejection);
			};

			return factory;
		}
	]);;