app.service('userServices', function($http, serverBaseUrl, $q){


	this.getUserById = function(id){
		return $http.get(serverBaseUrl + "api/users/" + id);		
	}

	// this.getUsers = function(name, role, contract, skip, limit){
	
	// 	if (!skip)
	// 		skip = 0;

	// 	if (!limit)
	// 		limit = 10;

	// 	var encodedUri = "?skip=" + skip + "&limit=" + limit;

	// 	if (name){
	// 		var encodedName = encodeURIComponent(name.toUpperCase());
	// 		encodedUri = encodedUri + "&name=" + encodedName;
	// 	}

	// 	if (contract){
	// 		encodedUri = encodedUri + "&contract=" + contract;
	// 	}

	// 	if (role){
	// 		encodedUri = encodedUri + "&role=" + role;
	// 	}

	// 	return $http.get(serverBaseUrl + "api/users" + encodedUri);

	// }

	this.getUsers = function(name, role){

		var deferred = $q.defer();
		var promise =  $http.get(serverBaseUrl + "api/users");
		promise.then(function(response){

			//se name e role sono vuoti allora restituisco tutto
			if (!role && !name){
				deferred.resolve(response.data);
				
			}else{

				var filtered = response.data.filter(function(a){
					if (!role)
						return a.name.indexOf(name) !== -1 
					if (!name)
						return a.role == role;

					return  a.name.indexOf(name) !== -1  && a.role == role;
				});

				deferred.resolve(filtered);				
			}


		})

		return deferred.promise;



		return $http.get(serverBaseUrl + "api/users");
	}



	this.createUser = function(user){
		return $http.post(serverBaseUrl + "api/users",  user);		
	}

	this.updateUser = function(user){
		return $http.put(serverBaseUrl + "api/users/" + user.id,  user);		
	}


})
