const query = require('../database/db')
const { dbFunctions } = require('../database/dbFunc')
const bcrypt = require('bcryptjs')


const userController = {
    patchUsers: async function (req, res, id) {
        console.log("Patching incoming...", req);
        try {
            const {name, email, location, pPic, phone} = req;

            const rows = await dbFunctions.execQueryWithReturn(
                `SELECT * FROM felhasznalok WHERE id = '${id}'`) || [];
            const hashed = rows[0].jelszo
            const favourites = rows[0].kedvencek
            const role = rows[0].role

            await query(`UPDATE felhasznalok SET nev= '${name}', email= '${email}', hely= '${location}', pPic= '${pPic}', jelszo= '${hashed}',
            telefonszam= '${phone}', kedvencek= '${favourites}', role= '${role}' WHERE id=${id}`);
            
            res.status(200).json({message: "User patched succesfully!"})
        } catch (err) {
            console.error("Error posting!", err.message);
        }
    },

    deleteUsers: async function (req, res, id) {
        console.log("Incoming delete on users...", req)
        try {
            await query(`
            DELETE FROM felhasznalok WHERE id = ${id}`)
            await query(`
            DELETE from hirdetesek WHERE tulajId = ${id}`)
            // delete from upload
            return res.status(200).json({message: `User with id: ${id} was deleted succesfully`})
        } catch (err) {
            console.error("Error deleting!", err.message);
        }
    },

    changePicture: async function (req,res,id) {
        console.log("Incoming PP change...", req.body)
        try {
            const {pPic} = req.body
            const rows = await dbFunctions.execQueryWithReturn(
                `SELECT * FROM felhasznalok WHERE id = ${id}`) || [];
            user = rows[0];
            await query(`UPDATE felhasznalok SET nev= '${user.nev}', email= '${user.email}', hely= '${user.hely}', pPic= '${pPic}', jelszo= '${user.jelszo}',
            telefonszam= '${user.telefonszam}', kedvencek= '${user.kedvencek}', role= '${user.role}' WHERE id=${id}`);
            res.status(200).json({message: "Updating profile was successful"})
        } catch (err) {
            console.error("Error changing pic!", err.message);
            res.status(500).json({error: "Internal server error!"})
        }
    },

    updateFavourites: async function (req,res,id) {
        console.log("Adding favourite...", req.body)
        try {
            const {adId} = req.body

            const rows = await dbFunctions.execQueryWithReturn(
                `SELECT * FROM felhasznalok WHERE id = ${id}`) || [];
            user = rows[0];
            const newFavourites = user.kedvencek + " " + adId + " +"

            await query(`
            UPDATE felhasznalok SET nev= '${user.nev}', email= '${user.email}', hely= '${user.hely}', pPic= '${user.pPic}', jelszo= '${user.jelszo}',
            telefonszam= '${user.telefonszam}', kedvencek= '${newFavourites}', role= '${user.role}' WHERE id=${id}`)
            res.status(200).json({message: "Ad marked as saved"})
        } catch (err) {
            console.error("Error updating favourites...", err.message)
            res.status(500).json({error: "Internal server error!"})
        }
    },

    removeFavourites: async function (req,res,id) {
        console.log("Removing favourite...", req.body)
        try {
            const { adId } = req.body
            const rows = await dbFunctions.execQueryWithReturn(
                `SELECT * FROM felhasznalok WHERE id = ${id}`) || [];
            user = rows[0];
            const newFavourites = (user.kedvencek + "").replace((" " + adId + " +"), '')
            
            await query(`
            UPDATE felhasznalok SET nev= '${user.nev}', email= '${user.email}', hely= '${user.hely}', pPic= '${user.pPic}', jelszo= '${user.jelszo}',
            telefonszam= '${user.telefonszam}', kedvencek= '${newFavourites}', role= '${user.role}' WHERE id=${id}`)
            res.status(200).json({message: "Ad removed from saved"})
            } catch (err) {
                console.error("Error updating favourites...", err.message)
                res.status(500).json({error: "Internal server error!"})
            } 
    },

    newPassword: async function (req,res,id) {
        console.log("Updating password...", req.body)
        try {
            const {password} = req.body
            const rows = await dbFunctions.execQueryWithReturn(
                `SELECT * FROM felhasznalok WHERE id = ${id}`) || [];
            user = rows[0];
            
            const hashedPassword = await bcrypt.hash(password, 10)
            await query(`
            UPDATE felhasznalok SET nev= '${user.nev}', email= '${user.email}', hely= '${user.hely}', pPic= '${user.pPic}', jelszo= '${hashedPassword}',
            telefonszam= '${user.telefonszam}', kedvencek= '${user.kedvencek}', role= '${user.role}' WHERE id=${id}`)
            res.status(200).json({message: "Password changed successfully"})
        } catch (err) {
            console.error("Error updating password...", err.message)
            res.status(500).json({error: "Internal server error!"})
        }
    },

    support: async function (req,res,id) {
        console.log("Incoming question...", req.body)
        try {
            const {title, question} = req.body
            await query(`
            INSERT INTO support (id, cim, kerdes, felhasznaloId) VALUES (null, '${title}', '${question}', '${id}')`)
            res.status(200).json({message: "We've received your question and will answer as soon as possible."})
        } catch (err) {
            console.error("Error contacting support...", err.message)
            res.status(500).json({error: "Internal server error!"})
        }
    },

    editUsers: async function (req, res) {
        console.log("Editing user incoming...", req);
        try {
            const {id, name, email, location, pPic, phone, role, favourites} = req.body;

            const rows = await dbFunctions.execQueryWithReturn(
                `SELECT * FROM felhasznalok WHERE id = '${id}'`) || [];
            const hashed = rows[0].jelszo

            await query(`UPDATE felhasznalok SET nev= '${name}', email= '${email}', hely= '${location}', pPic= '${pPic}', jelszo= '${hashed}',
            telefonszam= '${phone}', kedvencek= '${favourites}', role= '${role}' WHERE id=${id}`);
            
            res.status(200).json({message: "User edited succesfully!"})
        } catch (err) {
            console.error("Error posting!", err.message);
        }
    },

    removeUsers: async function (req, res) {
        console.log("Incoming delete on users...", req)
        try {
            const { id } = req.body;
            await query(`
            DELETE FROM felhasznalok WHERE id = ${id}`)
            await query(`
            DELETE from hirdetesek WHERE tulajId = ${id}`)
            // delete from upload
            return res.status(200).json({message: `User with id: ${id} was deleted succesfully`})
        } catch (err) {
            console.error("Error deleting!", err.message);
        }
    },
}

module.exports = {
    userController
}
