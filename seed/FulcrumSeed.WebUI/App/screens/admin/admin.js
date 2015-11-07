angular.module('fulcrumSeed.screens.admin', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin', {
					url: "/admin",
					templateUrl: "app/screens/admin/admin.html?v=" + APP_VERSION
				});

		})
	.controller('adminController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {


			$state.go('admin.userAccounts');
		}
	])
	.run(function ($rootScope, $state, $stateParams) {
		$rootScope.$state = $state;
		$rootScope.$stateParams = $stateParams;
	});