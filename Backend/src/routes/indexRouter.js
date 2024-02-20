const  express = require("express");
const {dbFunctions} = require('../database/dbFunc')
const verifyToken = require('../middlewares/jwtMiddleware');

const router = express.Router();

router.get("/", async function(_req, res, next) {
    try {
        res.render("index.html");
    } catch (err) {
        console.error("Error while loading in the main page!", err.message);
        next(err);
    }
});

router.get("/users", async function(_req, res, next) {
    try {
        res.json(await dbFunctions.getUsers());
    } catch (err) {
        console.error("Error while getting users!", err.message);
        next(err);
    }
});

router.get("/items", async function(_req, res, next) {
    try {
        res.json(await dbFunctions.getItems());
    } catch (err) {
        console.error("Error while getting items!", err.message);
        next(err);
    }
});

router.get("/pictures", async function(_req, res, next) {
    try {
        res.json(await dbFunctions.getPictures());
    } catch (err) {
        console.error("Error while getting pictures!", err.message);
        next(err);
    }
});

router.get("/settlements", async function(_req, res, next) {
    try {
        res.json(await dbFunctions.getSettlements());
    } catch (err) {
        console.error("Error while getting settlements!", err.message);
        next(err);
    }
});

router.get("/messages", async function(_req, res, next) {
    try {
        res.json(await dbFunctions.getMessages());
    } catch (err) {
        console.error("Error while getting messages!", err.message);
        next(err);
    }
});

router.get("/countys", async function(_req, res, next) {
    try {
        res.json(await dbFunctions.getCountys());
    } catch (err) {
        console.error("Error while getting countys!", err.message);
        next(err);
    }
});

router.post("/users/post", async function(req, res) {
    try {
        res.json(await dbFunctions.postUsers(req.body));
    } catch (err) {
        console.error("Error posting!", err.message);
    }
});

router.put("/users/put", async function(req, res) {
    try {
        res.json(await dbFunctions.putUsers(req.body));
    } catch (err) {
        console.error("Error updating!", err.message);
    }
}),

router.delete("/users/delete", async function(req, res) {
    try {
        res.json(await dbFunctions.deleteUsers(req.body))
    } catch (err) {
        console.error("Error deleting!", err.message);
    }
}),

router.post("/exec", async function(req, res) {
    try {
        res.json(await dbFunctions.execQuery(req.body))
    } catch (err) {
        console.error("Error executing query!", err.message);
    }
}),

router.post("/register", async function (req, res) {
    try {
        res.json(await dbFunctions.register(req.body))
        
    } catch (err) {
        console.error("Error while registering!", err.message)
    }
    res.status(201).json({message: "User registered successfully"})
}),

router.post("/login", async function (req, res) {
    try {
        res.json(await dbFunctions.login(req))
    } catch (err) {
        console.error("Error during login", err.message)
    }
}),

router.get('/protected-route', verifyToken, (req, res) => {
    res.json({ message: 'This is a protected route' });
});

module.exports = {
    router
}