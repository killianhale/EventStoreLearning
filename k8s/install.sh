helm install -n eventstore eventstore/eventstore --set persistence.enabled=true --set persistence.storageClass=eventstore

helm install -n mongodb stable/mongodb --set mongodbRootPassword=rootPassword1,mongodbUsername=apptApp,mongodbPassword=apptPassword1,mongodbDatabase=appointments,persistence.storageClass=appointmentdb,volumePermissions.enabled=true

curl -i -X POST "http://127.0.0.1:2113/projection/%24by_category/command/enable" -H "accept:application/json" -H "Content-Length:0" -u admin:changeit
curl -i -X POST "http://127.0.0.1:2113/projection/%24by_event_type/command/enable" -H "accept:application/json" -H "Content-Length:0" -u admin:changeit
