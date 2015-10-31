var app = angular.module(APP_ROOT_NS,
[
	'ui.router',
	APP_ROOT_NS + '.global.directives',
	APP_ROOT_NS + '.global.filters',
	APP_ROOT_NS + '.global.services',
	APP_ROOT_NS + '.global.shellController',
	APP_ROOT_NS + '.screens',
]);

angular.module(APP_ROOT_NS + '.global.directives', []);
angular.module(APP_ROOT_NS + '.global.filters', []);
angular.module(APP_ROOT_NS + '.global.services', []);
angular.module(APP_ROOT_NS + '.global.shellController', []);
angular.module(APP_ROOT_NS + '.screens', []);

// all screens must be included here
angular.module(APP_ROOT_NS + '.screens', [
	APP_ROOT_NS + '.screens.admin',
	APP_ROOT_NS + '.screens.login',
	APP_ROOT_NS + '.screens.profile',
	APP_ROOT_NS + '.screens.register                          ',
]);

angular.module(APP_ROOT_NS + '.screens.admin', []);
angular.module(APP_ROOT_NS + '.screens.login', []);
angular.module(APP_ROOT_NS + '.screens.profile', []);
angular.module(APP_ROOT_NS + '.screens.register', []);

app.config([
		'$stateProvider', '$urlRouterProvider', 'localStorageServiceProvider', '$httpProvider',
		function ($stateProvider, $urlRouterProvider, localStorageServiceProvider, $httpProvider) {

			$urlRouterProvider.otherwise("/");

			localStorageServiceProvider.setPrefix(APP_ROOT_NS);

			$httpProvider.interceptors.push('authInterceptorSvc');

		}
])
	.run([
		'$rootScope', 'authTokenSvc', 'authRedirectSvc',
		function ($rootScope, authTokenSvc, authRedirectSvc) {
			if (authTokenSvc.getToken()) {
				$rootScope.$broadcast("authenticated");
				$rootScope.$emit("authenticated");
			}

			$rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {
				// track the state the user wants to go to in case they fail authentication
				// and we need to pull it back out post-auth
				authRedirectSvc.setRedirect(toState, toStateParams);

				// ensure nav sync on every state change
				if (authTokenSvc.getToken()) {
					$rootScope.$broadcast("authenticated");
					$rootScope.$emit("authenticated");
				}
			});
		}
	]);