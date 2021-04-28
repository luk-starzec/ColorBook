const mongoose = require("mongoose");

const schemeSchema = new mongoose.Schema({
  userId: {
    type: mongoose.Schema.Types.ObjectId,
    required: true,
  },
  id: {
    type: String,
    required: true,
  },
  name: {
    type: String,
    required: true,
  },
  colors: [
    {
      name: {
        type: String,
      },
      color: {
        type: String,
        required: true,
      },
    },
  ],
  lastUpdate: {
    type: Date,
  },
});

module.exports = mongoose.model("Scheme", schemeSchema);
