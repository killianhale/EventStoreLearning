helm repo add eventstore https://eventstore.github.io/EventStore.Charts
helm repo update

mkdir -p /k8s/data/eventstore
kubectl create -f ./local-eventstore-pv.yaml