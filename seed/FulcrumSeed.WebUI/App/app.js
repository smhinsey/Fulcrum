var app = angular.module('fulcrumSeed',
[
	'ui.utils',
	'ui.bootstrap',
	'ui.router',
	'ui.grid',
	'ui.select',
	'pascalprecht.translate',
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
	'fulcrumSeed.screens.admin.users.accounts',
	'fulcrumSeed.screens.admin.users.roles',
	'fulcrumSeed.screens.admin.users.sessions',
	'fulcrumSeed.screens.admin.queries',
	'fulcrumSeed.screens.admin.commands',
	'fulcrumSeed.screens.profile',
	'fulcrumSeed.screens.register',
]);

angular.module('fulcrumSeed.screens.home', []);
angular.module('fulcrumSeed.screens.admin', []);
angular.module('fulcrumSeed.screens.admin.users.accounts', []);
angular.module('fulcrumSeed.screens.admin.users.roles', []);
angular.module('fulcrumSeed.screens.admin.users.sessions', []);
angular.module('fulcrumSeed.screens.admin.queries', []);
angular.module('fulcrumSeed.screens.admin.commands', []);
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
		'$rootScope', 'authSvc', 'authRedirectSvc',
		function ($rootScope, authSvc, authRedirectSvc) {

			$rootScope.$on('$stateChangeStart', function(event, toState, toStateParams) {
				// track the state the user wants to go to in case they fail authentication
				// and we need to pull it back out post-auth
				authRedirectSvc.setRedirect(toState, toStateParams);

				// ensure nav sync on every state change
				if (authSvc.isAuthorized()) {
					$rootScope.$broadcast('authenticated');
				} else {
					$rootScope.$broadcast('unauthenticated');

					// TODO: make configurable - set to true to force auth
					if (false) {
						$rootScope.$broadcast('show_login');
					}
				}
			});
		}
	]);