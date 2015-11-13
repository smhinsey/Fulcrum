angular.module('fulcrumSeed.global.services.profile', [])
	.service('profileSvc',
	[
		'$q', 'appSettings', '$timeout', '$http','localStorageService',
		function ($q, appSettings, $timeout, $http, localStorageService) {
			var getClaimsPromise = undefined;

			var setPromise = function () {
				var url = appSettings.apiBasePath + "auth/connect/userinfo";

				var promise = $http.get(url);

				getClaimsPromise = promise;
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

			// uses a promise to prevent multiple simultaneous requests
			// but successive calls do result in new requests if they are
			// made after the initial request returns
			this.getClaims = function () {

				if (getClaimsPromise == undefined) {
					setPromise();
				}

				if (getClaimsPromise.$$state.status === 1) {
					setPromise();
				}

				// TODO: investigate caching this - lots of edge cases around expiration tho
				return getClaimsPromise;
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