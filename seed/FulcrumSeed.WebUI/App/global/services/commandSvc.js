angular.module('fulcrumSeed.global.services.commands', [])
	.service('commandSvc',
	[
		'$q', 'appSettings', '$timeout', '$http',
		function($q, appSettings, $timeout, $http) {

			this.list = function (namespace) {

				var url = appSettings.apiBasePath + "api/commands/";

				if (namespace) {
					url += namespace;
				}

				var queryRequest = {
					url: url + "?v=" + APP_VERSION,
					method: 'GET',
				};

				return $http(queryRequest);
			}
			this.showRegistry = function () {

				var url = appSettings.apiBasePath + "api/commands/publication-registry";

				var queryRequest = {
					url: url + "?v=" + APP_VERSION,
					method: 'GET',
				};

				return $http(queryRequest);
			}
			this.getPublicationRecord = function (recordId) {

				var url = appSettings.apiBasePath + "api/commands/publication-registry/" + recordId;

				console.log('pub record from', url);

				var queryRequest = {
					url: url + "?v=" + APP_VERSION,
					method: 'GET',
				};

				return $http(queryRequest);
			}
			this.getSchema = function(name, namespace) {
				// TODO: add caching
				namespace = namespace == undefined ? "~" : namespace;

				var url = appSettings.apiBasePath + "api/commands/" + namespace + "/" + name + "";

				return $http.get(url);
			};
			this.publish = function(name, properties, opts) {
				var retries = 0;
				var maxRetries = 50;

				if (opts == undefined) {
					opts = {};
				}

				var namespace = opts.namespace == undefined ? "~" : opts.namespace;

				var deferred = $q.defer();

				var publish = function() {
					var url = appSettings.apiBasePath + "api/commands/" + namespace + "/" + name + "/publish";

					$http.post(url, properties)
						.success(function(pubRecord) {

							if (!pubRecord.status) {
								deferred.reject(pubRecord);
								return;
							}

							switch (pubRecord.status.toLowerCase()) {
							case "failed":
								deferred.reject(pubRecord);
								break;
							case "unpublished":
								deferred.reject(pubRecord);
								break;
							case "processing":
								// wait, then check again
								$timeout(function() {
									if (retries < maxRetries) {
										publish();
										retries++;
									}
								}, 500);
								break;
							case "complete":
								deferred.resolve(pubRecord);
							}

						})
						.error(function(pubRecord) {
							deferred.reject(pubRecord);
						});
				};

				publish();

				return deferred.promise;
			};
		}
	]);