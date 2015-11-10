angular.module('fulcrumSeed.global.services.queries', [])
	.service('querySvc',
	[
		'$q', 'appSettings', '$timeout', '$http',
		function($q, appSettings, $timeout, $http) {
			// TODO: add optional caching

			this.run = function(queryName, params, opts) {

				if (opts == undefined) {
					opts = {};
				}

				if (params == undefined) {
					params = {};
				}

				var namespace = opts.namespace == undefined ? "~" : opts.namespace;

				var url = appSettings.apiBasePath + "api/queries/" + namespace + "/" + queryName + "/results?";

				//return $http.get(url + $.param(params) + "&v=" + APP_VERSION);

				var queryRequest = {
					url: url + $.param(params) + "&v=" + APP_VERSION,
					method: 'GET',

				};

				if (opts.transformResponse) {
					queryRequest.transformResponse = opts.transformResponse;
				}

				if(opts.returnResultDirectly) {
					queryRequest.transformResponse = function (data) {
						var items = angular.fromJson(data);
						return items.results;
					}
				}

				return $http(queryRequest);
			};
		}
	]);