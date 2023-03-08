import sys
import matplotlib.pyplot as plt
import networkx as nx

G = nx.DiGraph()
dataset = []
edge_labels = {}

for i in range(1, len(sys.argv), 3):
	dataset += [[sys.argv[i], sys.argv[i + 1], sys.argv[i + 2]]]

for data in dataset:
	FROM = data[0]
	TO = data[1]
	VALUE = data[2]

	edge_labels[(FROM, TO)] = VALUE
	G.add_edge(FROM, TO, label=VALUE)

nx.set_edge_attributes(G, {('A', 'B'): {'color': 'red'}})

pos = nx.spring_layout(G)
plt.figure()
nx.draw(
	G, pos, edge_color='black', width=2,
	node_size=2000, node_color='pink', alpha=1,
	labels={node: node for node in G.nodes()}
)

nx.draw_networkx_edge_labels(
	G, pos,
	edge_labels=edge_labels,
	font_color='blue'
)

plt.savefig('/home/warek/RiderProjects/FLFA-Labs/plot.png')
plt.show()
