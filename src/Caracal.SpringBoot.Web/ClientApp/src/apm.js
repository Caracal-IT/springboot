const pageLoad = (page, closure) => {
    const transaction = Window.apm.startTransaction(`/${page}`, 'page-load', { managed: false });
    const httpSpan = transaction.startSpan(`/${page}`, 'page-load', { blocking: true });

    if(closure)
        closure();

    setTimeout(() => {
        httpSpan.end();
        transaction.end();
    }, 0);
}

export { pageLoad };
