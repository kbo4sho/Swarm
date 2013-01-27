package swarm.engine;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

public class PopulationSimulator {
	private Population population;

	private ArrayList<Individual> swarmInBirthOrder, swarmInXOrder,
			swarmInYOrder;

	public PopulationSimulator(Population population) {
		super();
		this.population = population;

		swarmInBirthOrder = new ArrayList<Individual>(population.size());
		swarmInXOrder = new ArrayList<Individual>(population.size());
		swarmInYOrder = new ArrayList<Individual>(population.size());
		
		for (Individual individual : population) {
			addIndividual(individual);
		}
	}

	public void stepSimulation(Collection<Individual> temporaryIndividuals, int weightOfTemporaries) {
		/*Individual tempSwarm, tempSwarm2;
		Parameters param;

		double tempX, tempY, tempX2, tempY2, tempDX, tempDY;
		double localCenterX, localCenterY, localDX, localDY, tempAx, tempAy, d;
		int n;*/

		ArrayList<Individual> neighbors = new ArrayList<Individual>();

		int numberOfSwarm = swarmInBirthOrder.size();

		for (int i = 0; i < numberOfSwarm; i++) {
			Individual currentInd = swarmInBirthOrder.get(i);
			Parameters param = currentInd.getGenome();
			double tempX = currentInd.getX();
			double tempY = currentInd.getY();

			double neighborhoodRadiusSquared = param.getNeighborhoodRadius()
					* param.getNeighborhoodRadius();

			neighbors.clear();

			// Detecting neighbors using sorted lists

			double minX = tempX - param.getNeighborhoodRadius();
			double maxX = tempX + param.getNeighborhoodRadius();
			double minY = tempY - param.getNeighborhoodRadius();
			double maxY = tempY + param.getNeighborhoodRadius();
			int minRankInXOrder = currentInd.getRankInXOrder();
			int maxRankInXOrder = currentInd.getRankInXOrder();
			int minRankInYOrder = currentInd.getRankInYOrder();
			int maxRankInYOrder = currentInd.getRankInYOrder();

			for (int j = currentInd.getRankInXOrder() - 1; j >= 0; j--) {
				if (swarmInXOrder.get(j).getX() >= minX)
					minRankInXOrder = j;
				else
					break;
			}
			for (int j = currentInd.getRankInXOrder() + 1; j < numberOfSwarm; j++) {
				if (swarmInXOrder.get(j).getX() <= maxX)
					maxRankInXOrder = j;
				else
					break;
			}
			for (int j = currentInd.getRankInYOrder() - 1; j >= 0; j--) {
				if (swarmInYOrder.get(j).getY() >= minY)
					minRankInYOrder = j;
				else
					break;
			}
			for (int j = currentInd.getRankInYOrder() + 1; j < numberOfSwarm; j++) {
				if (swarmInYOrder.get(j).getY() <= maxY)
					maxRankInYOrder = j;
				else
					break;
			}

			if (maxRankInXOrder - minRankInXOrder < maxRankInYOrder
					- minRankInYOrder) {
				for (int j = minRankInXOrder; j <= maxRankInXOrder; j++) {
					Individual tempSwarm2 = swarmInXOrder.get(j);
					if (currentInd != tempSwarm2)
						if (tempSwarm2.getRankInYOrder() >= minRankInYOrder
								&& tempSwarm2.getRankInYOrder() <= maxRankInYOrder) {
							if ((tempSwarm2.getX() - currentInd.getX())
									* (tempSwarm2.getX() - currentInd.getX())
									+ (tempSwarm2.getY() - currentInd.getY())
									* (tempSwarm2.getY() - currentInd.getY()) < neighborhoodRadiusSquared)
								neighbors.add(tempSwarm2);
						}
				}
			} else {
				for (int j = minRankInYOrder; j <= maxRankInYOrder; j++) {
					Individual tempSwarm2 = swarmInYOrder.get(j);
					if (currentInd != tempSwarm2)
						if (tempSwarm2.getRankInXOrder() >= minRankInXOrder
								&& tempSwarm2.getRankInXOrder() <= maxRankInXOrder) {
							if ((tempSwarm2.getX() - currentInd.getX())
									* (tempSwarm2.getX() - currentInd.getX())
									+ (tempSwarm2.getY() - currentInd.getY())
									* (tempSwarm2.getY() - currentInd.getY()) < neighborhoodRadiusSquared)
								neighbors.add(tempSwarm2);
						}
				}
			}

			/**/
			for (Individual ind : temporaryIndividuals) {
				if ((ind.getX() - currentInd.getX())
						* (ind.getX() - currentInd.getX())
						+ (ind.getY() - currentInd.getY())
						* (ind.getY() - currentInd.getY()) < neighborhoodRadiusSquared)
					for (int j = 0; j < weightOfTemporaries; j++)
						neighbors.add(ind);				
			}

			// simulating the behavior of swarm agents

			int n = neighbors.size();

			double tempAx, tempAy;
			if (n == 0) {
				tempAx = Math.random() - 0.5;
				tempAy = Math.random() - 0.5;
			}

			else {
				double localCenterX = 0, localCenterY = 0;
				double localDX = 0, localDY = 0;
				for (int j = 0; j < n; j++) {
					Individual tempSwarm2 = neighbors.get(j);
					localCenterX += tempSwarm2.getX();
					localCenterY += tempSwarm2.getY();
					localDX += tempSwarm2.getDx();
					localDY += tempSwarm2.getDy();
				}
				localCenterX /= n;
				localCenterY /= n;
				localDX /= n;
				localDY /= n;

				tempAx = tempAy = 0;

				tempAx += (localCenterX - tempX) * param.getC1();
				tempAy += (localCenterY - tempY) * param.getC1();

				tempAx += (localDX - currentInd.getDx()) * param.getC2();
				tempAy += (localDY - currentInd.getDy()) * param.getC2();

				for (int j = 0; j < n; j++) {
					Individual tempSwarm2 = neighbors.get(j);
					double tempX2 = tempSwarm2.getX();
					double tempY2 = tempSwarm2.getY();
					double d = (tempX - tempX2) * (tempX - tempX2) + (tempY - tempY2)
							* (tempY - tempY2);
					if (d == 0)
						d = 0.001;
					tempAx += (tempX - tempX2) / d * param.getC3();
					tempAy += (tempY - tempY2) / d * param.getC3();
				}

				if (Math.random() < param.getC4()) {
					tempAx += Math.random() * 10 - 5;
					tempAy += Math.random() * 10 - 5;
				}
			}

			currentInd.accelerate(tempAx, tempAy, param.getMaxSpeed());

			double tempDX = currentInd.getDx2();
			double tempDY = currentInd.getDy2();
			double d = Math.sqrt(tempDX * tempDX + tempDY * tempDY);
			if (d == 0)
				d = 0.001;
			currentInd.accelerate(tempDX * (param.getNormalSpeed() - d) / d
					* param.getC5(),
					tempDY * (param.getNormalSpeed() - d) / d * param.getC5(),
					param.getMaxSpeed());
		}
		
		updateInternalState();
	}

	private void updateInternalState() {
		for (Individual ind : swarmInBirthOrder) {
			ind.stepSimulation();
		}

		sortInternalLists();

		resetRanks();
	}

	private void sortInternalLists() {
		Collections.sort(swarmInXOrder, new Comparator<Individual>() {
			@Override
			public int compare(Individual o1, Individual o2) {
				return Double.compare(o1.getX(), o2.getX());
			}		
		});
		Collections.sort(swarmInYOrder, new Comparator<Individual>() {
			@Override
			public int compare(Individual o1, Individual o2) {
				return Double.compare(o1.getY(), o2.getY());
			}		
		});
	}

	private void addIndividual(Individual b) {
		swarmInBirthOrder.add(b);

		swarmInXOrder.add(b);		
		swarmInYOrder.add(b);

		sortInternalLists();

		/*if (swarmInXOrder.isEmpty()) {
			swarmInXOrder.add(b);
			swarmInYOrder.add(b);
		} else {
			if ((b.x - swarmInXOrder.get(0).x) < (swarmInXOrder
					.get(swarmInXOrder.size() - 1).x - b.x)) {
				i = 0;
				while (i < swarmInXOrder.size()) {
					if (swarmInXOrder.get(i).x >= b.x)
						break;
					i++;
				}
				swarmInXOrder.add(i, b);
			} else {
				i = swarmInXOrder.size();
				while (i > 0) {
					if (swarmInXOrder.get(i - 1).x <= b.x)
						break;
					i--;
				}
				swarmInXOrder.add(i, b);
			}

			if ((b.y - swarmInYOrder.get(0).y) < (swarmInYOrder
					.get(swarmInYOrder.size() - 1).y - b.y)) {
				i = 0;
				while (i < swarmInYOrder.size()) {
					if (swarmInYOrder.get(i).y >= b.y)
						break;
					i++;
				}
				swarmInYOrder.add(i, b);
			} else {
				i = swarmInYOrder.size();
				while (i > 0) {
					if (swarmInYOrder.get(i - 1).y <= b.y)
						break;
					i--;
				}
				swarmInYOrder.add(i, b);
			}

		}*/
	}

	private void resetRanks() {
		for (int i = 0; i < swarmInXOrder.size(); i++) {
			Individual tempSwarm = swarmInXOrder.get(i);
			if (tempSwarm.getRankInXOrder() != -1)
				tempSwarm.setRankInXOrder(i);
			else
				swarmInXOrder.remove(i--);
		}

		for (int i = 0; i < swarmInYOrder.size(); i++) {
			Individual tempSwarm = swarmInYOrder.get(i);
			if (tempSwarm.getRankInYOrder() != -1)
				tempSwarm.setRankInYOrder(i);
			else
				swarmInYOrder.remove(i--);
		}
	}

	public List<Individual> getSwarmInXOrder() {
		sortInternalLists();
		return Collections.unmodifiableList(swarmInXOrder);
	}

	public List<Individual> getSwarmInYOrder() {
		sortInternalLists();
		return Collections.unmodifiableList(swarmInYOrder);
	}

	public Population getPopulation() {
		return population;
	}
}
