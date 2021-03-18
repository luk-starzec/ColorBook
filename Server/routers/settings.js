const express = require("express");
const router = express.Router();
const userHelper = require("../helpers/userHelper");
const Setting = require("../models/setting");

router.post("/load", userHelper.getUser, async (req, res) => {
  const userId = res.user._id;
  try {
    var setting = await Setting.findOne({ userId: userId });

    if (setting == null) return res.sendStatus(404);

    res.json(setting);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

router.post("/save", userHelper.getUser, async (req, res) => {
  const userId = res.user._id;
  try {
    let setting = await Setting.findOne({ userId: userId });

    if (setting == null) setting = new Setting({ userId: userId });

    setting.lightBackgroundColor = req.body.lightBackgroundColor;
    setting.darkBackgroundColor = req.body.darkBackgroundColor;
    setting.lightTextColor = req.body.lightTextColor;
    setting.darkTextColor = req.body.darkTextColor;

    const newSetting = await setting.save();
    res.json(newSetting);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

module.exports = router;
