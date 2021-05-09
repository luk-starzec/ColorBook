const User = require("../models/user");

function getHash(pass) {
  return require("crypto").createHash("sha256").update(pass).digest("base64");
}

async function getUser(req, res, next) {
  const login = req.get("x-user");
  if (login == null) return res.sendStatus(404);

  let user;
  try {
    user = await User.findOne({ login });
    if (user == null) return res.sendStatus(404);

    const hash = req.get("x-access");
    const validHash = getHash(`${process.env.API_SECRET}${user.hash}`);
    if (hash != validHash) return res.sendStatus(401);
  } catch (err) {
    return res.status(500).json({ message: err.message });
  }
  res.user = user;
  next();
}

exports.getHash = getHash;
exports.getUser = getUser;
