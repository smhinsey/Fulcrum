angular.module('fulcrumSeed.screens.admin.userAccounts', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin.userAccounts', {
					url: "/users",
					templateUrl: "app/screens/admin/userAccounts/userAccounts.html?v=" + APP_VERSION
				});

		})
	.controller('userAccountsController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);