const express = require("express");
const router = express.Router();
const userHelper = require("../helpers/userHelper");
const User = require("../models/user");

router.get("/validate", userHelper.getUser, async (req, res) => {
  res.json({ dbId: res.user._id });
});

router.post("/new", async (req, res) => {
  const token = req.body.token;
  if (token != process.env.NEW_USER_TOKEN) {
    return res.sendStatus(401);
  }

  const login = req.body.login;
  try {
    const existingUser = await User.findOne({ login });
    if (existingUser)
      return res.status(400).json({ message: "login already exists" });

    const hash = userHelper.getHash(req.body.pass);

    const user = new User({
      login: login,
      hash: hash,
    });
    const newUser = await user.save();
    res.status(201).json({ dbId: newUser._id });
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

module.exports = router;
