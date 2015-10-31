angular.module('fulcrumSeed.screens.home', [])
		.config(function ($stateProvider) {

			$stateProvider
				.state('home', {
					url: "/home",
					templateUrl: "app/screens/home/home.html?v=" + APP_VERSION
				});

		})
	.controller('homeController', [
		'$scope', '$state', '$rootScope',
		function ($scope, $state, $rootScope) {

		}
	]);