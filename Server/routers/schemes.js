const express = require("express");
const router = express.Router();
const userHelper = require("../helpers/userHelper");
const Scheme = require("../models/scheme");

router.post("/loadLibrary", userHelper.getUser, async (req, res) => {
  const userId = res.user._id;
  try {
    const schemes = await Scheme.find({ userId: userId });

    if (schemes == null) return res.sendStatus(404);

    res.json(schemes);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

router.post("/saveLibrary", userHelper.getUser, async (req, res) => {
  const userId = res.user._id;
  const schemes = req.body.schemes;
  const dbSchemes = await Scheme.find({ userId: userId });

  const saveItems = schemes.map((scheme) => {
    let dbItem = dbSchemes.find((item) => item.id == scheme.id);

    dbItem = dbItem || new Scheme({ userId: userId, id: scheme.id });

    dbItem.name = scheme.name;
    dbItem.colors = scheme.colors;
    dbItem.lastUpdate = scheme.lastUpdate;
    return dbItem;
  });

  const deleteItemIds = dbSchemes
    .filter((item) => schemes.findIndex((s) => s.id == item.id) < 0)
    .map((item) => item.id);

  const promises = [];
  saveItems.forEach((scheme) => {
    promises.push(saveScheme(scheme, userId));
  });
  promises.push(deleteSchemes(deleteItemIds));

  const result = await Promise.all(promises);

  const errors = result
    .map((item) => item.error)
    .filter((item) => item != null);
  const saved = result.map((item) => item.saved).filter((item) => item != null);

  if (errors.length > 0)
    return res.status(500).json({ message: errors.join("; ") });

  res.json(saved);
});

router.post("/save", userHelper.getUser, async (req, res) => {
  const userId = res.user._id;
  const scheme = req.body.scheme;
  try {
    let dbScheme = await Scheme.findOne({ userId: userId });

    dbScheme = dbScheme || new Scheme({ userId: userId, id: scheme.id });

    dbScheme.name = scheme.name;
    dbScheme.colors = scheme.colors;
    dbScheme.lastUpdate = scheme.lastUpdate;

    const saved = await dbScheme.save();
    res.json(saved);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

router.post("/delete", userHelper.getUser, async (req, res) => {
  const userId = res.user._id;
  const schemeId = req.body.scheme;
  try {
    await Scheme.deleteOne({ userId: userId, id: schemeId });
    res.status(204);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

const saveScheme = async (scheme) => {
  try {
    const saved = await scheme.save();
    return { saved: saved };
  } catch (err) {
    return { error: err.message };
  }
};

const deleteSchemes = async (schemeIds) => {
  try {
    await Scheme.deleteMany({ id: { $in: schemeIds } });
    return {};
  } catch (err) {
    return { error: err.message };
  }
};

module.exports = router;
