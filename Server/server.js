require("dotenv").config();

const express = require("express");
const cors = require("cors");
const app = express();
const mongoose = require("mongoose");

mongoose.connect(process.env.DATABASE_URL, {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

const db = mongoose.connection;
db.on("error", (error) => console.log(error));
db.once("open", () => console.log("Connected to DB"));

app.use(express.json());
app.use(cors());

const testRouter = require("./routers/test");
app.use("/test", testRouter);

const hcRouter = require("./routers/hc");
app.use("/hc", hcRouter);

const usersRouter = require("./routers/users");
app.use("/users", usersRouter);

const settingsRouter = require("./routers/settings");
app.use("/settings", settingsRouter);

const schemesRouter = require("./routers/schemes");
app.use("/schemes", schemesRouter);

app.listen(process.env.APP_PORT, () => console.log("Server started"));
