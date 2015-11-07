angular.module('fulcrumSeed.screens.admin.userSessions', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.userSessions', {
					url: "/users",
					templateUrl: "app/screens/admin/userSessions/userSessions.html?v=" + APP_VERSION
				});

		})
	.controller('userSessionsController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);