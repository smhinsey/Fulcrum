angular.module('fulcrumSeed.screens.admin.userGroups', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.userGroups', {
					url: "/users",
					templateUrl: "app/screens/admin/userGroups/userGroups.html?v=" + APP_VERSION
				});

		})
	.controller('userGroupsController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);