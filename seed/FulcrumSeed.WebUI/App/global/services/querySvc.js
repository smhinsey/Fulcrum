﻿angular.module('fulcrumSeed.global.services.queries', [])
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

				var queryIdentifier = namespace + "/" + queryName;

				if (opts.skipNameWildcard != undefined) {
					if (opts.skipNameWildcard) {
						queryIdentifier = queryName;
					}
				}

				var url = appSettings.apiBasePath + "api/queries/" + queryIdentifier + "/results?";

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

			this.list = function(namespace) {

				var url = appSettings.apiBasePath + "api/queries/";

				if(namespace) {
					url += namespace;
				}


				var queryRequest = {
					url: url + "?v=" + APP_VERSION,
					method: 'GET',
				};

				return $http(queryRequest);
			}
		}
	]);