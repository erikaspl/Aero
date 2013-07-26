'use strict';

aero.factory('AeroStore', ['amplify', '$cookies', 'datacontext', function (amplify, $cookies, datacontext) {
    var customerIdTag = 'customerId';
    var selectedPartTag = 'selectedPartId';
	var aeroStore = {
		
		setCustomer: function (customerId) {
			amplify.store(customerIdTag, customerId);
		},

		getCustomer: function () {
			var defer = Q.defer();

			var customerId = amplify.store(customerIdTag);
			if (customerId == undefined) {
				var userName = $cookies['UserName'];
				customerId = datacontext.getCustomerByUserName(userName, false)
					.then(function (customer) {
						setCustomer(customer.id);
						defer.resolve(customer.id);
					});
			} else {
				defer.resolve(customerId);
			}

			return defer.promise;
		},


		selectPart: function (partId) {
		    amplify.store(selectedPartTag, partId);
		},

		getSelectedPart: function () {
		    return amplify.store(selectedPartTag);
		}
	}



	return aeroStore;
}]);