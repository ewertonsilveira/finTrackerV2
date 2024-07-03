
// src/index.ts
import express, { Express, Request, Response } from "express";
import router from "./routers"


/*
 * Create an Express application and get the
 * value of the PORT environment variable
 * from the `process.env`
 */
const app: Express = express();
const port = 3010;

/*
 * Create an Routing Express application 
 */
app.use('/api', router);

app.listen(port, () => {
  console.log(`Backend API  running at http://localhost:${port}`);
});