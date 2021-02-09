(function () {

    const db = idb.openDB('ColorBook', 1, {
        upgrade(db) {
            db.createObjectStore('metadata');
            db.createObjectStore('settings');
            db.createObjectStore('library', { keyPath: 'id' });
        },
    });

    window.localDataStore = {
        test: async () => {
            console.log("test");
            const aaa = await (await db).transaction('settings').store.get('current');
            console.log(aaa);
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
        //getFromIndex: async (storeName, indexName, keys) => {
        //    const cursor = await (await db).transaction(storeName).store.index(indexName).openCursor(null, direction);
        //    return (cursor && cursor.value) || null;
        //},
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
            //const store = (await db).transaction(storeName, 'readwrite').store;
            //JSON.parse(json).forEach(item => store.put(item));
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
        getTodayStatistics: async (storeName, keys) => {
            const results = [];

            const today = new Date().setHours(0, 0, 0, 0);;

            let cursor = await (await db).transaction(storeName).store.openCursor();
            while (cursor) {
                if (keys.includes(cursor.key)) {
                    const date = new Date(cursor.value.date).setHours(0, 0, 0, 0);
                    if (date == today)
                        results.push(cursor.value);
                }
                cursor = await cursor.continue();
            }
            console.log(results);
            return results;
        }
    };
})();
