const User = require("../models/user");

function getHash(pass) {
  return require("crypto").createHash("sha256").update(pass).digest("base64");
}

async function getUser(req, res, next) {
  const login = req.body.login;
  let user;
  try {
    const hash = getHash(req.body.pass);

    user = await User.findOne({ login, hash });

    if (user == null) return res.sendStatus(404);
  } catch (err) {
    return res.status(500).json({ message: err.message });
  }
  res.user = user;
  next();
}

exports.getHash = getHash;
exports.getUser = getUser;
