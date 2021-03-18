const express = require("express");
const router = express.Router();

router.get("/", (req, res) => {
  res.send("This is test");
});

router.get("/:data", (req, res) => {
  res.send(`Data is ${req.params.data}`);
});

module.exports = router;
