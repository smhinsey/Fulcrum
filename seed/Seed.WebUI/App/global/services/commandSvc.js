angular.module('fulcrumSeed.global.services.commands', [])
	.service('commandSvc',
	[
		'$q', 'appSettings', '$timeout', '$http',
		function ($q, appSettings, $timeout, $http) {

			this.getSchema = function(fullCommandName) {
				// check cache, if it misses, look for command by name
				// and return the json schema
				// if a 404 results, throw error asking if namespace is missing
			};
			this.validate = function(schema, command) {
				// use tv4 to validate command using schema.
				// look for query validation, execute it if present
			};
			this.publish = function(name, properties, opts) {
				var retries = 0;
				var maxRetries = 50;

				var namespace = opts.namespace == undefined ? "~" : opts.namespace;

				var deferred = $q.defer();

				var publish = function () {
					var url = appSettings.apiBasePath + "commands/" + namespace + "/" + name + "/publish";

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
								$timeout(function () {
									if(retries < maxRetries) {
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