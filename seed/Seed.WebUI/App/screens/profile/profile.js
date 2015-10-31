angular.module('fulcrumSeed.screens.profile', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('profile', {
					url: "/profile",
					templateUrl: "app/screens/profile/profile.html?v=" + APP_VERSION
				});

		})
	.controller('profileController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);