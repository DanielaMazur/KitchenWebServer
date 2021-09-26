# Run the app

1. Create the <i>kitchen</i> bridge network.
   (It should be created only once for both Severs)

```
$ docker network create kitchen
```

2. Build the project and create the <i>kitchen-server-image</i>. After that create a container (for the previously created image) called <i>kitchen-server-container</i>. The container will run over the <i>kitchen</i> network, on the port 8000. This configurations will allow the DiningHallServer to communicate with KitchenServer using the **kitchen-server-container** host.

```
$ docker build -t kitchen-server-image -f ./KitchenServer/Dockerfile .
$ docker run --net kitchen -d -p 8000:8000 --name kitchen-server-container kitchen-server-image
```
