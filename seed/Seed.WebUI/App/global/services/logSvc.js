angular.module(APP_ROOT_NS = '.global.services.log', [])
	.service('logSvc', [
		'$modal', '$timeout', '$http', 'appSettings',
		function ($modal, $timeout, $http, appSettings) {

			// TODO: implement this for all log levels - investigate $log first
			// it should support batching/flushing and and buffer using localstorage

			this.writeError = function (error) {
				//return $http.post(appSettings.apiBasePath + "errors/capture", { exception: error });
			};
		}
	])