var app = angular.module('fulcrumSeed',
[
	'ui.router', 'LocalStorageModule',
	'fulcrumSeed.global.directives',
	'fulcrumSeed.global.filters',
	'fulcrumSeed.global.services',
	'fulcrumSeed.global.services.auth',
	'fulcrumSeed.global.shell',
	'fulcrumSeed.screens',
]);

angular.module('fulcrumSeed.global.directives', []);
angular.module('fulcrumSeed.global.filters', []);
angular.module('fulcrumSeed.global.services', []);
angular.module('fulcrumSeed.global.services.auth', []);
angular.module('fulcrumSeed.global.shell', []);
angular.module('fulcrumSeed.screens', []);

// all screens must be included here
angular.module('fulcrumSeed.screens', [
	'fulcrumSeed.screens.home',
	'fulcrumSeed.screens.admin',
	'fulcrumSeed.screens.profile',
	'fulcrumSeed.screens.register',
]);

angular.module('fulcrumSeed.screens.home', []);
angular.module('fulcrumSeed.screens.admin', []);
angular.module('fulcrumSeed.screens.profile', []);
angular.module('fulcrumSeed.screens.register', []);

app.config([
		'$stateProvider', '$urlRouterProvider', 'localStorageServiceProvider', '$httpProvider',
		function($stateProvider, $urlRouterProvider, localStorageServiceProvider, $httpProvider) {

			$urlRouterProvider.otherwise("/home");

			localStorageServiceProvider.setPrefix('fulcrumSeed');

			$httpProvider.interceptors.push('authInterceptorSvc');

		}
	])
	.run([
		'$rootScope', 'authTokenSvc', 'authRedirectSvc',
		function($rootScope, authTokenSvc, authRedirectSvc) {
			if (authTokenSvc.getToken()) {
				$rootScope.$broadcast("authenticated");
				$rootScope.$emit("authenticated");
			}

			$rootScope.$on('$stateChangeStart', function(event, toState, toStateParams) {
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