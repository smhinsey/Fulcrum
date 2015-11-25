angular.module('fulcrumSeed.screens.admin', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('admin', {
					url: "/admin",
					templateUrl: "app/screens/admin/admin.html?v=" + APP_VERSION
				});

			$stateProvider
				.state('admin.users', {
					url: "/users",
					abstract: true,
					template: '<div ui-view></div>'
				});

		})
	.controller('adminController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {


			//$state.go('admin.users.accounts');
		}
	])
	.run(function ($rootScope, $state, $stateParams) {
		$rootScope.$state = $state;
		$rootScope.$stateParams = $stateParams;
	});