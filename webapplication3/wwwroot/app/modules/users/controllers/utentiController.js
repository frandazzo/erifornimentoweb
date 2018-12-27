app.controller('UtentiController', function($scope, $route, $rootScope, userServices){

	$rootScope.activeTab =  $route.current.activeTab;


	$scope.users = [];
	//parametri per la ricerca
	$scope.role = "USER";
	$scope.name = "";

	//variabile che indica il numero dei risultati ottenuti
	$scope.resultsNumber = 0;

	//funzione per la ricerca
	$scope.search = function(){
		
		$scope.loadData();
	}


	$scope.loadData = function(){
		var promise = userServices.getUsers($scope.name,$scope.role);
		promise.then(
			function(result){
				console.log(result);
				$scope.users = result;
				$scope.resultsNumber = result.length;
			},
			function(error){
				toastr.error('Errore', error.statusText );
			}
		);
	}


	$scope.loadData();
	
	
});