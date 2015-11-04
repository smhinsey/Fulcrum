angular.module('fulcrumSeed.global.services.profile', [])
	.service('profileSvc',
	[
		'$q', 'appSettings', '$timeout', '$http',
		function($q, appSettings, $timeout, $http) {
			// TODO: add caching

			this.getClaims = function() {
				var url = appSettings.apiBasePath + "auth/connect/userinfo";

				return $http.get(url);
			};
		}
	]);