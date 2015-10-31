angular.module('fulcrumSeed.screens.admin.users', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.users', {
					url: "/users",
					templateUrl: "app/screens/admin/users/users.html?v=" + APP_VERSION
				});

		})
	.controller('adminUsersController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);