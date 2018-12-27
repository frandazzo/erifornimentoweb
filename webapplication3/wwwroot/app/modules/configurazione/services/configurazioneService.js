app.service('configurazioneService', function($http, serverBaseUrl, $q){


	this.getConfigurazione = function(id){
		return $http.get(serverBaseUrl + "api/configuration");		
	}

	this.updateConfigurazione = function(conf){
		return $http.put(serverBaseUrl + "api/configuration/1",  conf);		
	}

	
})
