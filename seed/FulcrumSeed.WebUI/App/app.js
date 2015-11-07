var app = angular.module('fulcrumSeed',
[
	'ui.utils',
	'ui.bootstrap',
	'ui.router',
	'ui.grid',
	'angular-loading-bar',
	'LocalStorageModule',
	'schemaForm',
	'fulcrumSeed.global.directives',
	'fulcrumSeed.global.directives.claims',
	'fulcrumSeed.global.filters',
	'fulcrumSeed.global.services',
	'fulcrumSeed.global.services.auth',
	'fulcrumSeed.global.services.profile',
	'fulcrumSeed.global.services.commands',
	'fulcrumSeed.global.services.queries',
	'fulcrumSeed.global.shell',
	'fulcrumSeed.screens',
]);

angular.module('fulcrumSeed.global.directives', []);
angular.module('fulcrumSeed.global.directives.claims', []);
angular.module('fulcrumSeed.global.filters', []);
angular.module('fulcrumSeed.global.services', []);
angular.module('fulcrumSeed.global.services.auth', []);
angular.module('fulcrumSeed.global.services.profile', []);
angular.module('fulcrumSeed.global.services.commands', []);
angular.module('fulcrumSeed.global.services.queries', []);
angular.module('fulcrumSeed.global.shell', []);
angular.module('fulcrumSeed.screens', []);

// all screens must be included here
angular.module('fulcrumSeed.screens', [
	'fulcrumSeed.screens.home',
	'fulcrumSeed.screens.admin',
	'fulcrumSeed.screens.admin.userAccounts',
	'fulcrumSeed.screens.admin.userGroups',
	'fulcrumSeed.screens.admin.userSessions',
	'fulcrumSeed.screens.profile',
	'fulcrumSeed.screens.register',
]);

angular.module('fulcrumSeed.screens.home', []);
angular.module('fulcrumSeed.screens.admin', []);
angular.module('fulcrumSeed.screens.admin.userAccounts', []);
angular.module('fulcrumSeed.screens.admin.userGroups', []);
angular.module('fulcrumSeed.screens.admin.userSessions', []);
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