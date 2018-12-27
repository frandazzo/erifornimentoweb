app.controller('configurazioneController', function($scope, $route, $rootScope, configurazioneService, $routeParams, toastr, $location){

	$rootScope.activeTab =  $route.current.activeTab;
	
	configurazioneService.getConfigurazione()
	.then(function(response){
		$scope.configurazione=response.data;
	},
	function(error){
		toastr.error('Errore', error.statusText );
	});




	$scope.update= function(){

		
		configurazioneService.updateConfigurazione($scope.configurazione)
			.then(
				function(response){
					toastr.success('', "Aggiornamento completato");
					
				},function(error){
					
					toastr.error('Errore', error.data.error );
				}
			);
	}
});