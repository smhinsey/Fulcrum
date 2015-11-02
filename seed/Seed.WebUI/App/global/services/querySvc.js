angular.module('fulcrumSeed.global.services.queries', [])
	.service('querySvc',
	[
		'$q', 'appSettings', '$timeout', '$http',
		function($q, appSettings, $timeout, $http) {
			// TODO: add optional caching

			this.run = function(queryPath, queryName, params) {
				var url = appSettings.apiBasePath + "api/queries/" + queryPath + "/" + queryName + "/results?";

				return $http.get(url + $.param(params) + "&v=" + APP_VERSION);
			};
		}
	]);