helm install -n eventstore eventstore/eventstore --set persistence.enabled=true --set persistence.storageClass=eventstore

helm install -n mongodb stable/mongodb --set mongodbRootPassword=rootPassword1,mongodbUsername=apptApp,mongodbPassword=apptPassword1,mongodbDatabase=appointments,persistence.storageClass=appointmentdb,volumePermissions.enabled=true