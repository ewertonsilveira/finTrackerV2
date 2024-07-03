// src/index.ts
import express from "express";
const router = express.Router();
const fs = require('node:fs/promises');

async function readFile(fileName:string) {
  try {
    const data = await fs.readFile(fileName, { encoding: 'utf8' });
    return data;
  } catch (err) {
    console.error(err);
  }
}

let categories:any = null;
readFile('data/categories.json').then((v) => categories = JSON.parse(v));

router.get('/', (req, res) => {  
  res.send("FinTrackerApi");
});

// Categories
router.get('/categories', (req, res) => {
  //console.log(categories);
  res.status(200).json(categories);
});

router.get('/category/:id', (req, res) => {
  const userId = req.params.id;
  res.send(`Details of user ${userId}`);
});

router.post('/tags', (req, res) => {
  res.send('Create a new user');
});

router.put('/users/:id', (req, res) => {
  const userId = req.params.id;
  res.send(`Update user ${userId}`);
});

router.delete('/users/:id', (req, res) => {
  const userId = req.params.id;
  res.send(`Delete user ${userId}`);
});

export default router;