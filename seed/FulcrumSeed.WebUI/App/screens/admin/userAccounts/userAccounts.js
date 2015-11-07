angular.module('fulcrumSeed.screens.admin.userAccounts', [])
		.config(function ($stateProvider) {
			$stateProvider
				.state('admin.userAccounts', {
					url: "/users",
					templateUrl: "app/screens/admin/userAccounts/userAccounts.html?v=" + APP_VERSION
				});
		})
	.controller('userAccountsController', [
		'$scope', '$state', '$rootScope','querySvc',
		function ($scope, $state, $rootScope, querySvc) {

			var init = function() {

				querySvc.run('UserAccountQueries/ListUsers')
					.then(function (response) {
						console.log(response);
						$scope.users = response.data.results;
					}, function (error) {
						// TODO: ??
					});
			}

			init();
		}
	]);