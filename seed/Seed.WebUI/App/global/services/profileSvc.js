angular.module('fulcrumSeed.global.services.profile', [])
	.service('profileSvc',
	[
		'$q', 'appSettings', '$timeout', '$http','localStorageService',
		function ($q, appSettings, $timeout, $http, localStorageService) {
			// TODO: add caching

			this.getClaims = function() {
				var url = appSettings.apiBasePath + "auth/connect/userinfo";

				return $http.get(url);
			};
			var setProfile = function (profile) {
				localStorageService.set("user_profile", profile);
			};
			var getProfile = function () {
				return localStorageService.get("user_profile");
			};
			var clearProfile = function () {
				return localStorageService.remove("user_profile");
			};
			this.getProfile = function () {
				return getProfile();
			};
			this.setProfile = function (profile) {
				setProfile(profile);
			};
			this.clearProfile = function () {
				clearProfile();
			};
		}
	]);