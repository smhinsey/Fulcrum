// see http://plnkr.co/edit/CoIhRixz15WZNF4pFHCx?p=preview
// and http://stackoverflow.com/questions/24713242/using-ui-router-with-bootstrap-ui-modal

angular.module('fulcrumSeed.global.services.modalState', [])
	.provider('modalStateSvc', function($stateProvider) {
		var provider = this;
		this.$get = function() {
			return provider;
		};
		this.state = function(stateName, options) {
			var modalInstance;
			$stateProvider.state(stateName, {
				url: options.url,
				onEnter: function($modal, $state) {
					modalInstance = $modal.open(options);
					modalInstance.result['finally'](function() {
						modalInstance = null;
						if ($state.$current.name === stateName) {
							$state.go('^');
						}
					});
				},
				onExit: function() {
					if (modalInstance) {
						modalInstance.close();
					}
				}
			});
		};
	})