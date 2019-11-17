helm repo add eventstore https://eventstore.github.io/EventStore.Charts
helm repo update

mkdir -p /Users/Shared/k8s/data/eventstore
kubectl create -f ./local-eventstore-pv.yaml

mkdir -p /Users/Shared/k8s/data/mongodb
kubectl create -f ./local-mongodb-pv.yaml