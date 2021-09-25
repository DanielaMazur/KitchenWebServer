# Run the app

```
$ docker build -t kitchen-server-image -f ./KitchenServer/Dockerfile .
$ docker run -d -p 8000:8000 --name kitchen-server-container kitchen-server-image
```

Open http://localhost:8000/
