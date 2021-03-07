window.cryptoTools = {

    /*
     Returns "[iv]:[data]"
     [iv] is hexadecimal
     [data] is based64
    */
    encryptData: async (value, key) => {
        const iv = cryptoTools.getRandomIv(12);
        const theKey = await cryptoTools.getKey(key);

        const encryptedValue =
            await crypto.subtle.encrypt(
                {
                    name: 'AES-GCM',
                    iv
                },
                theKey,
                cryptoTools.encodeValue(value)
            );
        return `${cryptoTools.bufferToHex(iv)}:${cryptoTools.bufferToBase64(encryptedValue)}`;
    },

    /*
      Value format "[iv]:[data]"
      [iv] is hexadecimal
      [data] is based64
     */
    decryptData: async (value, key) => {
        const data = value.split(':');
        const iv = cryptoTools.hexToBuffer(data[0]);
        const theKey = await cryptoTools.getKey(key);

        const decryptedValue =
            await crypto.subtle.decrypt(
                {
                    name: 'AES-GCM',
                    iv
                },
                theKey,
                cryptoTools.base64ToBuffer(data[1])
            );
        return cryptoTools.decodeValue(decryptedValue);
    },

    getKey: async (password) => {
        return await crypto.subtle.importKey(
            'raw',
            await cryptoTools.getSha256(password),
            { name: 'AES-GCM' },
            false,
            ['encrypt', 'decrypt']
        );
    },

    encodeValue: (value) => new TextEncoder().encode(value),

    decodeValue: (value) => new TextDecoder().decode(value),

    bufferToBase64: (buffer) => btoa(String.fromCharCode(...new Uint8Array(buffer))),

    base64ToBuffer: (base64) => Uint8Array.from([...atob(base64)].map((c) => c.charCodeAt())).buffer,

    bufferToHex: (buffer) => [...new Uint8Array(buffer)].map((b) => b.toString(16).padStart(2, 0)).join(''),

    hexToBuffer: (hex) => new Uint8Array(hex.match(/.{1,2}/g).map((b) => Number.parseInt(b, 16))).buffer,

    getSha256: async (value) => await crypto.subtle.digest('SHA-256', cryptoTools.encodeValue(value)),

    getRandomIv: (length) => crypto.getRandomValues(new Uint8Array(length))

}