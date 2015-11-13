angular.module('fulcrumSeed.screens.admin.userSessions', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.users.sessions', {
					url: "/sessions",
					templateUrl: "app/screens/admin/users/sessions/sessions.html?v=" + APP_VERSION
				});

		})
	.controller('userSessionsController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);