const mongoose = require("mongoose");

const userSchema = new mongoose.Schema({
  login: {
    type: String,
    required: true,
  },
  hash: {
    type: String,
    required: true,
  },
});

module.exports = mongoose.model("User", userSchema);
