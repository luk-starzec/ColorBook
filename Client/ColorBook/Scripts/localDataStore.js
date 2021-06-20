import { openDB } from 'idb';

(function () {

    const db = openDB('ColorBook', 1, {
        upgrade(db) {
            db.createObjectStore('settings');
            db.createObjectStore('schemes');
            db.createObjectStore('deletedSchemes');
        },
    });

    window.localDataStore = {
        exists: async (storeName, key) => {
            const item = await (await db).transaction(storeName).store.get(key);
            return item !== undefined;
        },

        get: async (storeName, key) => (await db).transaction(storeName).store.get(key),

        getByKeys: async (storeName, keys) => {
            const results = [];
            let cursor = await (await db).transaction(storeName).store.openCursor();
            while (cursor) {
                if (keys.includes(cursor.key))
                    results.push(cursor.value);
                cursor = await cursor.continue();
            }
            console.log(results);
            return results;
        },

        getAll: async (storeName) => (await db).transaction(storeName).store.getAll(),

        getFirstFromIndex: async (storeName, indexName, direction) => {
            const cursor = await (await db).transaction(storeName).store.index(indexName).openCursor(null, direction);
            return (cursor && cursor.value) || null;
        },

        put: async (storeName, key, value) => (await db).transaction(storeName, 'readwrite').store.put(value, key === null ? undefined : key),

        putFromJson: async (storeName, json) => {
            const store = (await db).transaction(storeName, 'readwrite').store;
            Object.entries(JSON.parse(json)).forEach(item => store.put(item[1], item[0]));
        },

        putAllFromJson: async (storeName, json) => {
            console.log("test putAll");
            console.log(json);
            console.log(JSON.parse(json));
            Object.entries(JSON.parse(json)).forEach(item => console.log(item));
        },

        delete: async (storeName, key) => (await db).transaction(storeName, 'readwrite').store.delete(key),

        autocompleteKeys: async (storeName, text, maxResults) => {
            const results = [];
            let cursor = await (await db).transaction(storeName).store.openCursor(IDBKeyRange.bound(text, text + '\uffff'));
            while (cursor && results.length < maxResults) {
                results.push(cursor.key);
                cursor = await cursor.continue();
            }
            return results;
        },
    };
})();
