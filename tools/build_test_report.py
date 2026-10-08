from pathlib import Path
import html
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]
evidence = root / 'docs/03-codice/test-evidence'
source = evidence / 'results/unit-tests.trx'
ns = {'t': 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
run = ET.parse(source).getroot()
counters = run.find('t:ResultSummary/t:Counters', ns).attrib
rows = run.findall('t:Results/t:UnitTestResult', ns)
items = ''.join(f'<tr><td>{html.escape(r.attrib["testName"])}</td><td>{r.attrib["outcome"]}</td></tr>' for r in rows)
content = f'''<!doctype html><html lang="es"><meta charset="utf-8"><title>Códice - resultados xUnit</title>
<style>body{{font:16px Arial;background:#f3f7fa;color:#173349;margin:30px}}h1{{margin-bottom:8px}}.cards{{display:flex;gap:16px;margin:26px 0}}.card{{background:white;border:1px solid #ccdce5;padding:22px 35px;border-radius:8px}}strong{{font-size:34px;display:block;color:#15734b}}table{{width:100%;border-collapse:collapse;background:white;font-size:13px}}td,th{{padding:10px;border-bottom:1px solid #d9e5ec;text-align:left}}th{{background:#173349;color:white}}.source{{padding:16px;background:#e6f1ed;border-left:5px solid #15734b}}</style>
<h1>Códice | Ejecución de pruebas unitarias</h1><p>Fase 5 - xUnit - .NET 10 - configuración Release</p>
<div class="cards"><div class="card">Total<strong>{counters['total']}</strong></div><div class="card">Correctas<strong>{counters['passed']}</strong></div><div class="card">Fallidas<strong>{counters['failed']}</strong></div><div class="card">Omitidas<strong>{counters['notExecuted']}</strong></div></div>
<div class="source">Informe generado a partir de resultados reales TRX. Ejecución: {html.escape(run.attrib['name'])}.<br>Fuente: results/unit-tests.trx. La captura muestra este informe, no una terminal simulada.</div>
<p>Pruebas independientes de WPF y SQLite real. Los casos parametrizados generan varios resultados por método.</p>
<table><tr><th>Prueba y escenario ejecutados</th><th>Resultado</th></tr>{items}</table></html>'''
(evidence / 'test-report.html').write_text(content, encoding='utf-8')
print(f"Real TRX counters: {counters['total']} total, {counters['passed']} passed, {counters['failed']} failed")
